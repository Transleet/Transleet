<script lang="ts">
	import { onMount } from 'svelte';
	import axios from 'axios';
	import { goto } from '$app/navigation';
	onMount(async () => {
		let searchParams = new URLSearchParams(window.location.search);
		let state = searchParams.get('state');
		let response = await axios.post('/api/oauth/github_callback1?state=' + state);
		localStorage.setItem('token', response.data.token);
		goto('/');
	});
</script>