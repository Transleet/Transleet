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
  private userHub: HubConnection;

  static get instance(): SignalrHubs {
    if (this.INSTANCE === undefined) this.INSTANCE = new SignalrHubs();
    return this.INSTANCE;
  }

  private constructor() {
    this.projectHub = new HubConnectionBuilder()
      // eslint-disable-next-line @typescript-eslint/restrict-plus-operands
      .withUrl('https://localhost:7000/api' + '/hubs/projects', {
        accessTokenFactory: () => setting.token,
      })
      .configureLogging(LogLevel.Information)
      .build();
    this.projectHub.start().catch((err) => console.log(err));

    this.userHub = new HubConnectionBuilder()
      .withUrl('https://localhost:7000/api' + '/hubs/users', {
        accessTokenFactory: () => setting.token,
      })
      .configureLogging(LogLevel.Information)
      .build();
    this.userHub
      .start()
      .then(() => {
        setting.signalr = 'connected';
        cache.$reset();
      })
      .catch((err) => console.log(err));
    this.userHub.onclose(() => (setting.signalr = 'disconnected'));
  }

  public get ProjectHub(): HubConnection {
    return this.projectHub;
  }

  public get UserHub(): HubConnection {
    return this.userHub;
  }
}

export default SignalrHubs;
