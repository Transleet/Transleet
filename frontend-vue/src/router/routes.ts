import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: 'home', component: () => import('pages/Index.vue') },
      { path: 'login', component: () => import('pages/Login.vue') },
      { path: 'register', component: () => import('pages/Register.vue') },
      { path: 'search/:query', component: () => import('pages/Index.vue') },
      { path: 'search', component: () => import('pages/Index.vue') },
      {
        path: 'project/:id',
        component: () => import('pages/Project.vue'),
        children: [
          {
            path: 'general',
            name: 'general',
            component: () => import('components/project/Description.vue'),
          },
          { path: 'history', component: () => import('pages/Project.vue') },
          {
            path: 'files',
            component: () => import('components/project/Components.vue'),
          },
          { path: 'terms', component: () => import('pages/Project.vue') },
          { path: 'memory', component: () => import('pages/Project.vue') },
        ],
        redirect: { name: 'general' },
      },
    ],
    redirect: '/home',
  },
  {
    path: '/editor',
    component: () => import('layouts/EditorLayout.vue'),
    children: [
      {
        path: 'component/:project/:component',
        component: () => import('pages/editor/ComponentEditor.vue'),
      },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/Error404.vue'),
  },
];

export default routes;
