#!/bin/bash
# Script to decrypt all the secrets files.
# Club App|ETS
pwd
openssl version -v

# Decrypt Firebase secret files for production.
if [[ -n "$ENCRYPTED_FIREBASE_PROD_PASSWORD" ]]; then
  mkdir -p ./PortalApi/local
  echo "Decoding Firebase Settings secret file"
  openssl enc -aes-256-cbc -pbkdf2 -d -k "$ENCRYPTED_FIREBASE_PROD_PASSWORD" -in ./encryptedFiles/firebase-settings-prod.json.enc -out ./PortalApi/local/firebase-settings.json -md md5
  # openssl enc -aes-256-cbc -pbkdf2 -e -k "$ENCRYPTED_FIREBASE_PROD_PASSWORD" -in ./PortalApi/local/firebase-settings-prod.json -out ./encryptedFiles/firebase-settings-prod.json.enc -md md5
  echo "Decoding Firebase service account file"
  openssl enc -aes-256-cbc -pbkdf2 -d -k "$ENCRYPTED_FIREBASE_PROD_PASSWORD"-in ./encryptedFiles/adminsdk-prod.json.enc -out ./PortalApi/local/adminsdk.json -md md5
  # openssl enc -aes-256-cbc -pbkdf2 -e -k "$ENCRYPTED_FIREBASE_PROD_PASSWORD" -in ./PortalApi/local/firebase-settings-prod.json -out ./encryptedFiles/adminsdk-prod.json.enc -md md5
fi

# Decrypt Firebase secret files for development.
if [[ -n "$ENCRYPTED_FIREBASE_DEV_PASSWORD" ]]; then
  mkdir -p ./PortalApi/local
  echo "Decoding Firebase Settings secret file"
  openssl enc -aes-256-cbc -pbkdf2 -d -k "$ENCRYPTED_FIREBASE_DEV_PASSWORD" -in ./encryptedFiles/firebase-settings-dev.json.enc -out ./PortalApi/local/firebase-settings.json -md md5
  # openssl enc -aes-256-cbc -pbkdf2 -e -k "$ENCRYPTED_FIREBASE_DEV_PASSWORD" -in ./PortalApi/local/firebase-settings-dev.json -out ./encryptedFiles/firebase-settings-dev.json.enc -md md5
  echo "Decoding Firebase service account file"
  openssl enc -aes-256-cbc -pbkdf2 -d -k "$ENCRYPTED_FIREBASE_DEV_PASSWORD" -in ./encryptedFiles/adminsdk-dev.json.enc -out ./PortalApi/local/adminsdk.json -md md5
  # openssl enc -aes-256-cbc -pbkdf2 -e -k "$ENCRYPTED_FIREBASE_DEV_PASSWORD" -in ./PortalApi/local/adminsdk-dev.json -out ./encryptedFiles/adminsdk-dev.json.enc -md md5
fi
