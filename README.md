# Portal API
An API dedicated to the portal API.

## Setup for DEV :sailboat: or PROD :rocket:

- To access some features you will need the Firebase admin sdk, these files are encrypted. To decrypt them you will have to do two simple steps:

You need to ask an admin user to provide the dev password for encrypted files saved in the pasword manager, then run:

```sh
# Run this for dev environment
export ENCRYPTED_FIREBASE_DEV_PASSWORD="ABCDE12345";
# Or run this for prod environment
export ENCRYPTED_FIREBASE_PROD_PASSWORD="ABCDE12345";

# Then run:
scripts/decrypt.sh
```


## Setup for local :building_construction:
There is two way to setup a local environment project.

### Remote firebase
In most case, you will want a remote firebase project. If you want more flexibility or want to develop without needing an internet connection, you can check [this](#local-firebase-instance) out.

Otherwise, follow these steps to create a new project in the firebase console:

1. Create an authentication method in your firebase console.

    1. Go to **Authentication** page.
    2. Go to the tab **Sign-in method**.

    ![image](https://user-images.githubusercontent.com/25663435/163913316-19b01f17-baf6-43b8-92de-f4a0a33e672a.png)

    3. Click on **email/password** and click on the checkbox before saving it.

    ![image](https://user-images.githubusercontent.com/25663435/163913346-832dfac0-c139-40be-8f23-c926d2fe1182.png)
    
2. Create a new cloud firestore project.
    1. Go to **Firestore Database** page.
    2. Click on **Create new database**.

    ![image](https://user-images.githubusercontent.com/25663435/163913389-0de0a538-9a1b-48b8-a3ff-0f711e7591eb.png)

    3. Choose production mode.
    
    ![image](https://user-images.githubusercontent.com/25663435/163913410-ff6cdd96-6644-4179-a8ba-e2c2e390e377.png)

    4. Choose the nearest region possible (**us-east1** is a possible good candidate).
    
    ![image](https://user-images.githubusercontent.com/25663435/163913418-38c5400d-9811-4d74-8975-ee237c3f48f4.png)

    5. Click on **Create**.

3. Generate your admin sdk token.
    1. Go to **Project Settings** page.
    2. Go to **Service Accounts** tab.

    ![image](https://user-images.githubusercontent.com/25663435/163913439-d526335d-9ee6-4e6d-9374-1fd4ef9f487f.png)

    3. Click on **Generate new private key**.
    
    ![image](https://user-images.githubusercontent.com/25663435/163913462-f2d2e0b3-1a4c-4905-aa06-f48f96cfba9f.png)

    4. download the key and save it to a file `PortalApi/local/admin-sdk.json`

4. Get the project custom information.
    1. Go to **Project Settings** page in **Global parameter** tab.
    2. Save the following fields: `Project id` and `API Web Key`.
    ![image](https://user-images.githubusercontent.com/25663435/163913859-60a4697d-2194-4ff1-a444-f61953e58976.png)

    4. Create a file `PortalApi/local/firebase-settings.json` with the following content:

    ```json
    {
      "FirebaseSettings": {
        "ProjectId": "/* insert ProjectId value here */",
        "ApiKey": "/* insert ApiKey value here */"
      }
    }
    ```
### Local firebase instance
You can use the firebase emulator to create a local firebase instance. follows [these guidelines](https://firebase.google.com/docs/emulator-suite/install_and_configure) to setup a cloud firestore and a firbase authentication service.

