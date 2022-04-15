<template>
  <div class="q-pa-md">
    <q-stepper v-model="step" vertical color="primary" animated>
      <q-step :name="1" title="set name" icon="settings" :done="step > 1">
        114

        <q-stepper-navigation>
          <q-btn @click="step = 2" color="primary" label="Continue" />
        </q-stepper-navigation>
      </q-step>

      <q-step
        :name="2"
        title="choose a picture"
        caption="Optional"
        icon="create_new_folder"
        :done="step > 1"
      >
        114

        <q-stepper-navigation>
          <q-btn @click="step = 3" color="primary" label="Continue" />
          <q-btn
            flat
            @click="step = 1"
            color="primary"
            label="Back"
            class="q-ml-sm"
          />
        </q-stepper-navigation>
      </q-step>

      <q-step :name="3" title="choose file type" icon="assignment">
        114
        <q-stepper-navigation>
          <q-btn
            @click="step = 4"
            color="primary"
            label="Continue"
            class="q-ml-sm"
          />
        </q-stepper-navigation>
      </q-step>

      <q-step :name="4" title="set member rule" icon="add_comment">
        114

        <q-stepper-navigation>
          <q-btn color="primary" label="Finish" @click="create" />
          <q-btn
            flat
            @click="step < 1"
            color="primary"
            label="Back"
            class="q-ml-sm"
          />
        </q-stepper-navigation>
      </q-step>
    </q-stepper>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import SignalrHubs from 'src/signalr';
import { useCacheStore } from '../store/cache';
import { Project } from '../models/Project';
const cache = useCacheStore();
let step = ref(1);

async function create() {
  const p: Project = await SignalrHubs.instance.ProjectHub.invoke('Create', {
    displayName: 'test' + (Math.random() * 10).toString(),
    description: (Math.random() * 10).toString() + 'test',
    avatar:
      'https://images.unsplash.com/photo-1649453366204-2481ecd8225b?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2487&q=80',
  });
  cache.home.projects.push(p);
  cache.home.show = false;
}
</script>
