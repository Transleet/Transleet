import { ref, defineComponent, watch } from 'vue';
<template>
  <q-splitter
    v-model="splitterModel"
    style="height: 500px"
    v-if="cache.project.edit"
  >
    <template #separator>
      <q-avatar
        color="primary"
        text-color="white"
        size="28px"
        icon="fas fa-arrows-alt-h"
      />
    </template>

    <template #before>
      <div class="q-pa-md">
        <textarea v-model="desc" :rows="20" class="fit q-pa-sm" />
      </div>
    </template>

    <template #after>
      <div class="q-pa-md" style="height: 467px">
        <q-markdown
          :src="desc"
          :plugins="plugins"
          class="fit bordered q-pa-sm"
        />
      </div>
    </template>
  </q-splitter>
  <q-markdown v-else :src="desc" :plugins="plugins"></q-markdown>
</template>

<script lang="js">
import { defineComponent, ref } from 'vue';
import { useCacheStore } from '../../store/cache';
import '@quasar/quasar-ui-qmarkdown/dist/index.css';
import { QMarkdown } from '@quasar/quasar-ui-qmarkdown';
import * as emoji from 'markdown-it-emoji';
import * as abbreviation from 'markdown-it-abbr';
import * as deflist from 'markdown-it-deflist'
import * as footnote from 'markdown-it-footnote'
import * as insert from 'markdown-it-ins'
import * as mark from 'markdown-it-mark'
import * as subscript from 'markdown-it-sub'
import * as superscript from 'markdown-it-sup'
import * as taskLists from 'markdown-it-task-lists'
import * as mermaid from '@datatraccorporation/markdown-it-mermaid'
import SignalrHubs from 'src/signalr';

export default defineComponent({
  name: 'Description',
  components: {
    QMarkdown,
  },
  setup() {
    const cache = useCacheStore();
    const plugins = [emoji,abbreviation,deflist,footnote,insert,mark,subscript,superscript,taskLists,mermaid];

    const splitterModel = ref(50);
    const desc = ref('');
    desc.value = cache.project.main.description;
    cache.$subscribe((m,s) => {
      if (s.project.edit === false) {
        if(s.project.main.description === desc.value) {
          return;
        }
        s.project.main.description = desc.value;
        SignalrHubs.instance.ProjectHub.invoke('Update', cache.project.main).catch((e) => {
          console.error(e);
        });
      }
    });

    return {
      desc,
      plugins,
      cache,
      splitterModel,
    };
  },
});
</script>
