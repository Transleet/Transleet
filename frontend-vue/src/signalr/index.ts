import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from '@microsoft/signalr';
import { useSettingStore } from '../store/setting';
import { useCacheStore } from '../store/cache';

const cache = useCacheStore();
const setting = useSettingStore();

class SignalrHubs {
  private static INSTANCE: SignalrHubs;
  private projectHub: HubConnection;

  static get instance(): SignalrHubs {
    if (this.INSTANCE === undefined) this.INSTANCE = new SignalrHubs();
    return this.INSTANCE;
  }

  private constructor() {
    this.projectHub = new HubConnectionBuilder()
      .withUrl('https://localhost:7000/api/hubs/projects', {
        accessTokenFactory: () => setting.token,
      })
      .configureLogging(LogLevel.Information)
      .build();
    this.projectHub
      .start()
      .then(() => {
        setting.signalr.projectHub = 'connected';
        cache.$reset();
      })
      .catch((err) => console.log(err));
    this.projectHub.onclose(
      () => (setting.signalr.projectHub = 'disconnected')
    );
  }

  public get ProjectHub(): HubConnection {
    return this.projectHub;
  }
}

export default SignalrHubs;
