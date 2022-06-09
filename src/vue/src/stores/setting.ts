import { defineStore } from 'pinia';

export const useSettingStore = defineStore('setting', {
  state: () => ({
    dark: 'auto' as boolean | 'auto',
    token: '',
    logined: false,
    signalr: 'disconnected' as 'disconnected' | 'connected',
  }),
  actions: {
    getAllSignalrState(): 'disconnected' | 'connected' {
      return this.signalr;
    },
  },
  persist: {
    enabled: true,
    strategies: [{ key: 'setting', storage: localStorage }],
  },
});
