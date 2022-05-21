/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-return */
import { defineStore } from 'pinia';
import { Component, Project } from 'src/lib/api';

export const useCacheStore = defineStore('cache', {
  state: () => ({
    home: {
      projects: new Map<string, Project>(),
      show: false,
    },
    project: {
      main: null as Project | null,
      components: new Map<string, Component>(),
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
    getProjectComponents(): Component[] {
      return [...this.project.components.values()];
    },
    setProjectComponents(components: Component[]): void {
      this.project.components = new Map(
        components.map((component) => {
          if (component.key === undefined) {
            component.key = '';
          }
          return [component.key, component];
        })
      );
    },
  },
});
