<template>
  <q-page padding>
    <div v-if="isLoading">
      <q-skeleton
        type="rect"
        height="24px"
        animation="blink"
        class="q-mx-md q-my-md"
      ></q-skeleton>
      <div class="row-md q-col-gutter-md">
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
        {{ cache.project.main?.displayName }}
      </div>
      <div class="row-md q-col-gutter-md">
        <q-list class="col-2 q-mx-md q-my-md" padding>
          <q-item
            class="flex-center"
            clickable
            :to="'/project/' + route.params.id + '/general'"
          >
            <q-item-section avatar>
              <q-icon name="analytics"></q-icon>
            </q-item-section>
            <q-item-section>
              <q-item-label>概览</q-item-label>
            </q-item-section>
          </q-item>
          <q-item
            class="flex-center"
            clickable
            :to="'/project/' + route.params.id + '/history'"
          >
            <q-item-section avatar>
              <q-icon name="history"></q-icon>
            </q-item-section>
            <q-item-section>
              <q-item-label>历史记录</q-item-label>
            </q-item-section>
          </q-item>
          <q-item
            class="flex-center"
            clickable
            :to="'/project/' + route.params.id + '/files'"
          >
            <q-item-section avatar>
              <q-icon name="description"></q-icon>
            </q-item-section>
            <q-item-section>
              <q-item-label>文件</q-item-label>
            </q-item-section>
          </q-item>
          <q-item
            class="flex-center"
            clickable
            :to="'/project/' + route.params.id + '/terms'"
          >
            <q-item-section avatar>
              <q-icon name="storage"></q-icon>
            </q-item-section>
            <q-item-section>
              <q-item-label>术语库</q-item-label>
            </q-item-section>
          </q-item>
          <q-item
            class="flex-center"
            clickable
            :to="'/project/' + route.params.id + '/memory'"
          >
            <q-item-section avatar>
              <q-icon name="turned_in"></q-icon>
            </q-item-section>
            <q-item-section>
              <q-item-label>记忆库</q-item-label>
            </q-item-section>
          </q-item>
        </q-list>
        <q-card id="card" class="col q-mx-md q-my-md">
          <q-card-section>
            <div class="row-md">
              <div class="col-auto">
                <q-circular-progress
                  show-value
                  font-size="10px"
                  class="q-ma-md"
                  :value="60"
                  size="120px"
                  :thickness="0.25"
                  color="primary"
                  track-color="grey-3"
                >
                  <q-avatar size="100px">
                    <img :src="cache.project.main?.avatar + ''" />
                  </q-avatar>
                </q-circular-progress>
              </div>
              <div class="col q-mt-md q-mr-md q-pl-md">
                <div class="row justify-between">
                  <div class="row">
                    <q-chip
                      color="secondary"
                      text-color="white"
                      icon="translate"
                      >语言：
                    </q-chip>
                    <q-chip color="primary" text-color="white" icon="done_all"
                      >翻译进度：
                    </q-chip>
                  </div>
                  <div>
                    <q-chip
                      :color="cache.project.edit ? 'primary' : 'grey-3'"
                      :text-color="cache.project.edit ? 'white' : 'black'"
                      :icon="cache.project.edit ? 'done' : 'edit'"
                      clickable
                      square
                      @click="cache.project.edit = !cache.project.edit"
                      >{{ cache.project.edit ? '保存' : '编辑' }}</q-chip
                    >
                  </div>
                </div>
                <div class="row q-mt-md">
                  <q-chip color="grey-3" text-color="black" square>标签</q-chip>
                  <q-chip color="primary" text-color="white">1234 </q-chip>
                </div>
                <div class="row">
                  <q-chip color="positive" text-color="white" square>
                    协议
                  </q-chip>
                </div>
              </div>
            </div>
          </q-card-section>
          <q-card-section>
            <router-view></router-view>
          </q-card-section>
        </q-card>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router';
import { onMounted, ref } from 'vue';
import SignalrHubs from 'src/signalr';
import { useCacheStore } from '../stores/cache';

const route = useRoute();
const cache = useCacheStore();

let isLoading = ref(true);

onMounted(async () => {
  while (SignalrHubs.instance.ProjectHub.state !== 'Connected') {
    await new Promise((resolve) => setTimeout(resolve, 100));
  }

  isLoading.value = false;
});
</script>

<script lang="ts">
export default {
  name: 'ProjectPage',
}
</script>

<style scoped lang="scss"></style>
