<template>
  <div class="mx-auto q-mx-md q-my-md">
    <q-grid x-gap="12" y-gap="8" cols="2" sm="1" xs="1">
      <q-grid-item>
        <q-card>
          <q-card-section>
            <div class="text-h6">我的关注</div>
            <q-btn color="primary" label="add">
              <q-popup-proxy v-model="cache.home.show">
                <new-project></new-project>
              </q-popup-proxy>
            </q-btn>
            <q-btn label="rm" @click="remove"></q-btn>
          </q-card-section>
          <q-card-section> 114 </q-card-section>
        </q-card>
        <q-card class="q-mt-md">
          <q-card-section>
            <div class="text-h6">精选项目</div>
          </q-card-section>
          <q-card-section>
            <div class="row q-gutter-md">
              <template v-for="p in cache.getHomeProjects()" :key="p.key">
                <project-card :project="p"></project-card>
              </template>
            </div>
          </q-card-section>
        </q-card>
      </q-grid-item>
      <q-grid-item>
        <q-card>
          <q-card-section>
            <div class="text-h6">公告</div>
          </q-card-section>
          <q-card-section> 114 </q-card-section>
        </q-card>
      </q-grid-item>
    </q-grid>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import SignalrHubs from 'src/signalr';
import NewProject from 'src/components/NewProject.vue';
import { useCacheStore } from '../store/cache';
import QGrid from 'src/components/quasar-extend/QGrid.vue';
import QGridItem from 'src/components/quasar-extend/QGridItem.vue';
import ProjectCard from '../components/ProjectCard.vue';
import { ProjectsService } from '../lib/api/services/ProjectsService';

const cache = useCacheStore();

SignalrHubs.instance.ProjectHub.state;

onMounted(async () => {
  let projects = await ProjectsService.getProjects();
  setTimeout(() => {
    cache.setHomeProjects(projects);
    console.log(projects);
  }, 50);

  while (SignalrHubs.instance.ProjectHub.state !== 'Connected') {
    await new Promise((resolve) => setTimeout(resolve, 100));
  }
  SignalrHubs.instance.ProjectHub.stream(
    'SubscribeProjectNotification'
  ).subscribe({
    // eslint-disable-next-line @typescript-eslint/no-misused-promises
    next: async (notification) => {
      console.log(notification);
      // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
      if (notification.operation === 0) {
        // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
        let proj = await ProjectsService.getProject(<string>notification.id);
        if(proj.id === undefined) return;
        cache.home.projects.set(proj.id, proj);
      } else {
        // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
        cache.home.projects.delete(<string>notification.id);
      }
    },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    complete: () => {},
    error: (err) => {
      console.log(err);
    },
  });
});

async function remove() {
  while (SignalrHubs.instance.ProjectHub.state !== 'Connected') {
    await new Promise((resolve) => setTimeout(resolve, 100));
  }
  console.log('123');
}
</script>
