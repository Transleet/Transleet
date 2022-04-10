/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-return */
import { defineStore } from 'pinia';
import { Project } from '../models/Project';
export const useCacheStore = defineStore('cache', {
  state: () => ({
    home: {
      projects: [] as Project[],
      show: false,
    },
  }),
  actions: {},
});
