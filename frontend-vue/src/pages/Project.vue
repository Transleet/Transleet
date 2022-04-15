<template>
  <q-page padding class="row-md q-col-gutter-md">
    <q-list class="col-2 q-mx-md q-my-md" padding>
      <q-item class="flex-center" clickable>
        <q-item-section avatar>
          <q-icon name="analytics"></q-icon>
        </q-item-section>
        <q-item-section>
          <q-item-label>概览</q-item-label>
        </q-item-section>
      </q-item>
      <q-item class="flex-center" clickable>
        <q-item-section avatar>
          <q-icon name="history"></q-icon>
        </q-item-section>
        <q-item-section>
          <q-item-label>历史记录</q-item-label>
        </q-item-section>
      </q-item>
      <q-item class="flex-center" clickable>
        <q-item-section avatar>
          <q-icon name="announcement"></q-icon>
        </q-item-section>
        <q-item-section>
          <q-item-label>公告</q-item-label>
        </q-item-section>
      </q-item>
      <q-item class="flex-center" clickable>
        <q-item-section avatar>
          <q-icon name="description"></q-icon>
        </q-item-section>
        <q-item-section>
          <q-item-label>文件</q-item-label>
        </q-item-section>
      </q-item>
      <q-item class="flex-center" clickable>
        <q-item-section avatar>
          <q-icon name="storage"></q-icon>
        </q-item-section>
        <q-item-section>
          <q-item-label>术语库</q-item-label>
        </q-item-section>
      </q-item>
      <q-item class="flex-center" clickable>
        <q-item-section avatar>
          <q-icon name="turned_in"></q-icon>
        </q-item-section>
        <q-item-section>
          <q-item-label>记忆库</q-item-label>
        </q-item-section>
      </q-item>
    </q-list>
    <q-card class="col q-mx-md q-my-md">
      <q-card-section>
        <div class="row justify-between">
          <div class="text-h4">{{ project?.displayName }}</div>
          <q-img :src="project?.avatar" width="24px"  ratio="1"></q-img>
        </div>
      </q-card-section>
      <q-card-section> 114 </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { LocationQueryValue, useRouter, useRoute } from 'vue-router';
import { onMounted, ref } from 'vue';
import { Project } from '../models/Project';
import SignalrHubs from 'src/signalr';

const route = useRoute();
const router = useRouter();

let isLoading = ref(true);
let project = ref<Project | undefined>(undefined);

onMounted(async () => {
  const id = route.params.id;
  while (SignalrHubs.instance.ProjectHub.state !== 'Connected') {
    await new Promise((resolve) => setTimeout(resolve, 100));
  }
  async function getProjectInfo(
    id: LocationQueryValue | LocationQueryValue[] | undefined
  ) {
    if (id === undefined || id === null) router.back();
    let p: Project = await SignalrHubs.instance.ProjectHub.invoke('Get', id);
    project.value = p;
    isLoading.value = false;
  }

  await Promise.all([getProjectInfo(id)]);
});
</script>
