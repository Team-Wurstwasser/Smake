import tkinter as tk
from tkinter import filedialog, messagebox
from pathlib import Path
import os

from cryptography.hazmat.primitives import hashes
from cryptography.hazmat.primitives.kdf.pbkdf2 import PBKDF2HMAC
from cryptography.hazmat.primitives.ciphers.aead import AESGCM
from cryptography.hazmat.backends import default_backend

PASSWORD = b"djsghiowrhurt9iwezriwehgfokweh9tfhwoirthweoihtoeriwh"
ITERATIONS = 100_000
KEY_LEN = 32
SALT_LEN = 16
NONCE_LEN = 12
TAG_LEN = 16


def derive_key(password: bytes, salt: bytes) -> bytes:
    kdf = PBKDF2HMAC(
        algorithm=hashes.SHA256(),
        length=KEY_LEN,
        salt=salt,
        iterations=ITERATIONS,
        backend=default_backend()
    )
    return kdf.derive(password)


def aes_encrypt(plaintext: bytes) -> bytes:
    salt = os.urandom(SALT_LEN)
    nonce = os.urandom(NONCE_LEN)
    key = derive_key(PASSWORD, salt)

    aesgcm = AESGCM(key)

    ciphertext_with_tag = aesgcm.encrypt(
        nonce,
        plaintext,
        None
    )

    ciphertext = ciphertext_with_tag[:-TAG_LEN]
    tag = ciphertext_with_tag[-TAG_LEN:]

    return salt + nonce + ciphertext + tag


def aes_decrypt(data: bytes) -> bytes:
    if len(data) < SALT_LEN + NONCE_LEN + TAG_LEN:
        raise ValueError("Invalid encrypted data")

    salt = data[:SALT_LEN]
    nonce = data[SALT_LEN:SALT_LEN + NONCE_LEN]

    tag = data[-TAG_LEN:]
    ciphertext = data[SALT_LEN + NONCE_LEN:-TAG_LEN]

    key = derive_key(PASSWORD, salt)
    aesgcm = AESGCM(key)

    ciphertext_with_tag = ciphertext + tag

    return aesgcm.decrypt(
        nonce,
        ciphertext_with_tag,
        None
    )
    
class CryptoGUI(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Spielstand Editor")
        self.geometry("500x200")

        self.file_path = tk.StringVar()

        tk.Label(self, text="Datei:").pack(anchor="w", padx=10, pady=5)
        frame = tk.Frame(self)
        frame.pack(fill="x", padx=10)
        tk.Entry(frame, textvariable=self.file_path, width=50).pack(side="left", expand=True, fill="x")
        tk.Button(frame, text="Durchsuchen", command=self.browse_file).pack(side="left", padx=5)

        btn_frame = tk.Frame(self)
        btn_frame.pack(pady=20)
        tk.Button(btn_frame, text="Verschlüsseln", command=self.encrypt_file, width=20).pack(side="left", padx=10)
        tk.Button(btn_frame, text="Entschlüsseln", command=self.decrypt_file, width=20).pack(side="left", padx=10)

    def browse_file(self):
        path = filedialog.askopenfilename()
        if path:
            self.file_path.set(path)

    def encrypt_file(self):
        if not self.file_path.get():
            messagebox.showerror("Fehler", "Bitte Datei auswählen")
            return
        try:
            in_path = Path(self.file_path.get())
            data = in_path.read_bytes()
            encrypted = aes_encrypt(data)

            if in_path.suffix == ".dec":
                out_path = in_path.with_name(in_path.stem + ".enc")
                in_path.unlink()
            else:
                out_path = in_path.with_suffix(in_path.suffix + ".enc")

            out_path.write_bytes(encrypted)
            messagebox.showinfo("Erfolg", f"Datei verschlüsselt:\n{out_path}")
        except Exception as e:
            messagebox.showerror("Fehler", str(e))

    def decrypt_file(self):
        if not self.file_path.get():
            messagebox.showerror("Fehler", "Bitte Datei auswählen")
            return
        try:
            in_path = Path(self.file_path.get())
            data = in_path.read_bytes()
            decrypted = aes_decrypt(data)

            if in_path.suffix == ".enc":
                out_path = in_path.with_name(in_path.stem + ".dec")
                in_path.unlink()
            else:
                out_path = in_path.with_suffix(in_path.suffix + ".dec")

            out_path.write_bytes(decrypted)
            messagebox.showinfo("Erfolg", f"Datei entschlüsselt:\n{out_path}")
        except Exception as e:
            messagebox.showerror("Fehler", str(e))


if __name__ == "__main__":
    app = CryptoGUI()
    app.mainloop()
