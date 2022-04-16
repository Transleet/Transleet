import adapter from '@sveltejs/adapter-node';
import WindiCSS from 'vite-plugin-windicss';
import preprocess from 'svelte-preprocess';

/** @type {import('@sveltejs/kit').Config} */
const config = {
	preprocess: preprocess(),

	kit: {
		prerender: {
			default: true
		},
		adapter: adapter(),
		vite: {
			plugins: [WindiCSS()]
		}
	}
};

export default config;
