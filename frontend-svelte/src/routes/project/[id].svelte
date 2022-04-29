<script lang="ts">
	import { getStores, navigating, page, session, updated } from '$app/stores';
	import { OpenAPI, ProjectsService, type Project } from '$lib/api';
	import { onMount } from 'svelte';
	let project: Project;
	let backend_base_url = import.meta.env.VITE_BACKEND_BASE_URL;
	let frontend_base_url = import.meta.env.VITE_FRONTEND_BASE_URL;
	onMount(() => {
		
	});
	export async function load({ params, fetch, session, stuff }) {
        OpenAPI.BASE = backend_base_url;
		OpenAPI.TOKEN = async () => {
			return localStorage.getItem('token');
		};  
		const id = params.id;
		project = await ProjectsService.getProject(id);
		console.log(project);
	}
</script>

<h1>{project.name}</h1>
