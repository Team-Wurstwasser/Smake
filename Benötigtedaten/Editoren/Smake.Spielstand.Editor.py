#!/usr/bin/env python3
import tkinter as tk
from tkinter import filedialog, messagebox
from pathlib import Path
import os

from cryptography.hazmat.primitives import hashes, padding
from cryptography.hazmat.primitives.kdf.pbkdf2 import PBKDF2HMAC
from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes
from cryptography.hazmat.backends import default_backend

# --- Konfiguration ---
PASSWORD = b"djsghiowrhurt9iwezriwehgfokweh9tfhwoirthweoihtoeriwh"
ITERATIONS = 100_000
KEY_LEN = 32
IV_LEN = 16
SALT_LEN = 16  # Zufälliger Salt Länge


def derive_key_iv(password: bytes, salt: bytes):
    total_len = KEY_LEN + IV_LEN
    kdf = PBKDF2HMAC(
        algorithm=hashes.SHA256(),
        length=total_len,
        salt=salt,
        iterations=ITERATIONS,
        backend=default_backend()
    )
    okm = kdf.derive(password)
    return okm[:KEY_LEN], okm[KEY_LEN:]


def aes_encrypt(plaintext: bytes) -> bytes:
    salt = os.urandom(SALT_LEN)
    key, iv = derive_key_iv(PASSWORD, salt)

    padder = padding.PKCS7(algorithms.AES.block_size).padder()
    padded = padder.update(plaintext) + padder.finalize()

    cipher = Cipher(algorithms.AES(key), modes.CBC(iv), backend=default_backend())
    encryptor = cipher.encryptor()
    ciphertext = encryptor.update(padded) + encryptor.finalize()

    return salt + ciphertext


def aes_decrypt(data: bytes) -> bytes:
    if len(data) < SALT_LEN:
        raise ValueError("Ungültige verschlüsselte Datei")

    salt = data[:SALT_LEN]
    ciphertext = data[SALT_LEN:]
    key, iv = derive_key_iv(PASSWORD, salt)

    cipher = Cipher(algorithms.AES(key), modes.CBC(iv), backend=default_backend())
    decryptor = cipher.decryptor()
    padded_plain = decryptor.update(ciphertext) + decryptor.finalize()

    unpadder = padding.PKCS7(algorithms.AES.block_size).unpadder()
    return unpadder.update(padded_plain) + unpadder.finalize()


# --- GUI ---
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
                # "save.bin.dec" -> "save.bin.enc"
                out_path = in_path.with_name(in_path.stem + ".enc")
                in_path.unlink()  # alte .dec löschen
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
                # "save.bin.enc" -> "save.bin.dec"
                out_path = in_path.with_name(in_path.stem + ".dec")
                in_path.unlink()  # alte .enc löschen
            else:
                out_path = in_path.with_suffix(in_path.suffix + ".dec")

            out_path.write_bytes(decrypted)
            messagebox.showinfo("Erfolg", f"Datei entschlüsselt:\n{out_path}")
        except Exception as e:
            messagebox.showerror("Fehler", str(e))


if __name__ == "__main__":
    app = CryptoGUI()
    app.mainloop()
