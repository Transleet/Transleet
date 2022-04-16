<script lang="ts">
	import { onMount } from 'svelte';
	import axios from 'axios';
	import { goto } from '$app/navigation';
	let backend_base_url = import.meta.env.VITE_BACKEND_BASE_URL;
	let frontend_base_url = import.meta.env.VITE_FRONTEND_BASE_URL;
	onMount(async () => {
		let searchParams = new URLSearchParams(window.location.search);
		let state = searchParams.get('state');
		let response = await axios.get(backend_base_url + '/api/oauth/github_callback1?state=' + state);
		localStorage.setItem('token', response.data.token);
		goto(frontend_base_url + '/');
	});
</script>
