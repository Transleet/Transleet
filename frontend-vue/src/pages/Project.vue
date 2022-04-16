<template>
  <q-page padding>
    <div v-if="isLoading">
      <q-skeleton
        type="rect"
        height="24px"
        animation="blink"
        class="q-mx-md q-my-md"
      ></q-skeleton>
      <div class="row-md q-col-gutter-md flex flex-center">
        <q-list class="col-2 q-mx-md q-my-md" padding>
          <q-item class="flex-center" clickable>
            <q-item-section avatar>
              <q-skeleton
                type="QAvatar"
                size="24px"
                animation="blink"
              ></q-skeleton>
            </q-item-section>
            <q-item-section>
              <q-skeleton type="rect" animation="blink"></q-skeleton>
            </q-item-section>
          </q-item>
          <q-item class="flex-center" clickable>
            <q-item-section avatar>
              <q-skeleton
                type="QAvatar"
                size="24px"
                animation="blink"
              ></q-skeleton>
            </q-item-section>
            <q-item-section>
              <q-skeleton type="rect" animation="blink"></q-skeleton>
            </q-item-section>
          </q-item>
          <q-item class="flex-center" clickable>
            <q-item-section avatar>
              <q-skeleton
                type="QAvatar"
                size="24px"
                animation="blink"
              ></q-skeleton>
            </q-item-section>
            <q-item-section>
              <q-skeleton type="rect" animation="blink"></q-skeleton>
            </q-item-section>
          </q-item>
          <q-item class="flex-center" clickable>
            <q-item-section avatar>
              <q-skeleton
                type="QAvatar"
                size="24px"
                animation="blink"
              ></q-skeleton>
            </q-item-section>
            <q-item-section>
              <q-skeleton type="rect" animation="blink"></q-skeleton>
            </q-item-section>
          </q-item>
          <q-item class="flex-center" clickable>
            <q-item-section avatar>
              <q-skeleton
                type="QAvatar"
                size="24px"
                animation="blink"
              ></q-skeleton>
            </q-item-section>
            <q-item-section>
              <q-skeleton type="rect" animation="blink"></q-skeleton>
            </q-item-section>
          </q-item>
          <q-item class="flex-center" clickable>
            <q-item-section avatar>
              <q-skeleton
                type="QAvatar"
                size="24px"
                animation="blink"
              ></q-skeleton>
            </q-item-section>
            <q-item-section>
              <q-skeleton type="rect" animation="blink"></q-skeleton>
            </q-item-section>
          </q-item>
        </q-list>
        <q-card class="col q-mx-md q-my-md">
          <q-card-section>
            <q-skeleton height="76vh" class="q-mr-md q-mb-md" animation="blink"
          /></q-card-section>
        </q-card>
      </div>
    </div>
    <div v-else>
      <div
        class="q-mx-md q-my-md"
        style="font-size: 24px; font-weight: bold; padding-left: 24px"
      >
        {{ project?.displayName }}
      </div>
      <div class="row-md q-col-gutter-md flex flex-center">
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
        <q-card id="card" class="col q-mx-md q-my-md" style="height: 82vh">
          <q-card-section>
            <q-circular-progress
              show-value
              font-size="10px"
              class="q-ma-md"
              :value="60"
              size="100px"
              :thickness="0.25"
              color="primary"
              track-color="grey-3"
            >
              <q-avatar size="80px">
                <img src="https://cdn.quasar.dev/logo-v2/svg/logo.svg" />
              </q-avatar>
            </q-circular-progress>
          </q-card-section>
        </q-card>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { LocationQueryValue, useRouter, useRoute } from 'vue-router';
import { onMounted, ref } from 'vue';
import { Project } from '../models/Project';
import SignalrHubs from 'src/signalr';

const route = useRoute();
const router = useRouter();

let isLoading = ref(false);
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
  isLoading.value = false;
});
</script>

<style scoped lang="scss"></style>
