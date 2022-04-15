import { createPinia } from 'pinia';
import { boot } from 'quasar/wrappers';
import piniaPluginPersist from 'pinia-plugin-persist';

const pinia = createPinia();
pinia.use(piniaPluginPersist);

export default boot(({ app }) => {
  app.use(pinia);
});

export { pinia };
