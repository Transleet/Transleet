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
import { useCacheStore } from '../stores/cache';
import { Project } from 'src/lib/api';
import { ProjectsService } from '../lib/api/services/ProjectsService';
const cache = useCacheStore();
let step = ref(1);

async function create() {
  console.log('create')
  const pt: Project = {
    name:'TestProject',
    description:'114514'
  };
  // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
  let p:Project = await ProjectsService.createProject(pt);
  if(p.id === undefined) return;
  cache.home.projects.set(p.id,p);
  cache.home.show = false;
}
</script>
