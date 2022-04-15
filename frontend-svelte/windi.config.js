import { defineConfig } from 'windicss/helpers';
import daisyui from 'daisyui';
export default defineConfig({
	plugins: [daisyui],
	extract: {
		include: ['src/**/*.{html,svelte}']
	}
});
