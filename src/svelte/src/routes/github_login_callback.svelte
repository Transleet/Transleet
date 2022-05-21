<script lang="ts">
	import { onMount } from 'svelte';
	import axios from 'axios';
	import { goto } from '$app/navigation';
	import { page } from '$app/stores';
	import { userStore } from '$lib/stores';
	let backend_base_url = import.meta.env.VITE_BACKEND_BASE_URL;
	let frontend_base_url = import.meta.env.VITE_FRONTEND_BASE_URL;
	onMount(async () => {
		let state = $page.url.searchParams.get('state');
		let response = await axios.get(backend_base_url + '/api/oauth/github_callback1?state=' + state);
		$userStore.token = response.data.token;
		$userStore.id = response.data.id;
		$userStore.isLogin =  true;
		goto(frontend_base_url + '/');
	});
</script>
