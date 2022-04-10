import { defineStore } from 'pinia';
import { Project } from '../models/Project';
export const useCacheStore = defineStore('cache', {
  state: () => ({
    home: {
      projects: [] as Project[],
      show: false,
    },
  }),
});
