import { defineStore } from 'pinia';

export const useSettingStore = defineStore('setting', {
  state: () => ({
    dark: 'auto' as boolean | 'auto',
    token: '',
  }),
  actions: {},
  persist: {
    enabled: true,
    strategies: [{ key: 'setting', storage: localStorage }],
  },
});
