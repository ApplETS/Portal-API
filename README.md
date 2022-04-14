# Portal API
An API dedicated to the portal.

# Setup

Setup nécessaire au fonctionnement du Firebase Admin SDK (surement à ajouter au Readme):
1. Télécharger le fichier json des secrets dans l'app Firebase. Paramètre du projet -> Compte de service -> Générer une nouvelle clé privée.
2. Placer le fichier a un endroit sécuritaire et créer une variable d'environnement GOOGLE_APPLICATION_CREDENTIALS pointant vers l'emplacement du fichier

Pour Linux/Mac:
export GOOGLE_APPLICATION_CREDENTIALS="/home/user/Downloads/service-account-file.json"

Pour Windows(Powershell):
$env:GOOGLE_APPLICATION_CREDENTIALS="C:\Users\username\Downloads\service-account-file.json"

Référence: https://firebase.google.com/docs/admin/setup

Setup pour le fonctionnement du FirebaseService
Afin de protéger les variables sensibles (apikey, secrets, etc...), .NET possède un Secret Manager. Voici comme le setup:
Référence: https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows

1. Créer un fichier json avec le format suivant:
```
{
"FirebaseSettings": {
    "ProjectId": "",
    "ApiKey": ""
  }
}
```
Et enregistrer le sous le nom secrets.json

2. Aller chercher le valeur du tag `UserSecretsId` dans le .csproj du projet. Si la valeur est manquante exécuter la commande suivante: `dotnet user-secrets init`.

3. Aller placer le fichier secrets.json dans un des emplacements suivants selon le os:

Windows:
%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json

Linux/MacOS:
~/.microsoft/usersecrets/<user_secrets_id>/secrets.json
