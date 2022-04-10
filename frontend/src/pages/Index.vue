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
            <template v-for="p in cache.home.projects" :key="p.key">
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
import { Project } from 'src/models/Project';
import { onMounted, watch } from 'vue';
import SignalrHubs from 'src/signalr';
import NewProject from 'src/components/NewProject.vue';
import { useCacheStore } from '../store/cache';
import QGrid from 'src/components/quasar-extend/QGrid.vue';
import QGridItem from 'src/components/quasar-extend/QGridItem.vue';
import ProjectCard from '../components/ProjectCard.vue';

const cache = useCacheStore();

onMounted(async () => {
  while (SignalrHubs.instance.ProjectHub.state !== 'Connected') {
    await new Promise((resolve) => setTimeout(resolve, 100));
  }
  SignalrHubs.instance.ProjectHub.stream('GetTopTen').subscribe({
    next: (p) => {
      if (p !== null) {
        console.log(p);
        cache.home.projects.push(<Project>p);
      }
    },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    complete: () => {},
    error: (err) => {
      console.log(err);
    },
  });
});

watch(cache.home.projects, () => {
  console.log(cache.home.projects);
});

async function remove() {
  let projectToBeRemoved = cache.home.projects.shift();
  if (projectToBeRemoved === undefined) return;
  await SignalrHubs.instance.ProjectHub.invoke(
    'Delete',
    projectToBeRemoved.key
  );
}
</script>
