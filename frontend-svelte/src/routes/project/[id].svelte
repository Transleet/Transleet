<script lang="ts">
	import { page } from '$app/stores';
	import { ProjectsService, type Project } from '$lib/api';
	import { onMount } from 'svelte';
	let project: Project;
	onMount(async () => {
		project = await ProjectsService.getProject($page.params.id);
	});
</script>

{#if project}
	<h1>{project.name}</h1>
	<h2>{project.description}</h2>
{:else}
	<p>loading...</p>
{/if}
