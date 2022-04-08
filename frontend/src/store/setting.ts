import { defineStore } from 'pinia';

export const useSettingStore = defineStore('setting', {
  state: () => ({
    dark: 'auto' as boolean | 'auto',
    token: '',
    signalr: {
      projectHub: 'disconnected' as 'disconnected' | 'connected',
    },
  }),
  actions: {
    getAllSignalrState(): 'disconnected' | 'connected' {
      return this.signalr.projectHub;
    },
  },
  persist: {
    enabled: true,
    strategies: [{ key: 'setting', storage: localStorage }],
  },
});
