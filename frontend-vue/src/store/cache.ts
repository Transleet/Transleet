/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-return */
import { defineStore } from 'pinia';
import { Project } from 'src/lib/api';
export const useCacheStore = defineStore('cache', {
  state: () => ({
    home: {
      projects: new Map<string, Project>(),
      show: false,
    },
    project: {
      main: null as Project | null,
      edit: false,
    },
    users: {
      allUser: [] as string[],
    },
  }),
  actions: {
    getHomeProjects(): Project[] {
      return [...this.home.projects.values()];
    },
    setHomeProjects(projects: Project[]): void {
      this.home.projects = new Map(
        projects.map((project) => {
          if (project.id === undefined) {
            project.id = '';
          }
          return [project.id, project];
        })
      );
    },
  },
});
