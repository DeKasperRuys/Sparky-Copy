export class EnvironmentUrls {
    public isLocal(local) {
        if (local) {
          return 'https://localhost:5001/';
        } else {
          return 'https://sparky1920api.azurewebsites.net/';
        }
    }
}
