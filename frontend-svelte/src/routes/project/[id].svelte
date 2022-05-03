<script lang="ts">
	import { page } from '$app/stores';
	import { ComponentsService, ProjectsService, type Component, type Project } from '$lib/api';
	import { onMount } from 'svelte';
	let project: Project;
	let componentName;
	onMount(async () => {
		project = await ProjectsService.getProject($page.params.id);
	});
	async function createComponent(name: string) {
		let component = await ComponentsService.createComponent({ name });
		project.components = [...project.components, component.id];
		await ProjectsService.updateProject(project);
	}
</script>

<div>
	<div>
		{#if project}
			<h1>{project.name}</h1>
			<h2>{project.description}</h2>
		{:else}
			<p>loading...</p>
		{/if}
	</div>
	<div>
		<div>Components</div>
		<script lang="ts">
		</script>
		<input bind:value={componentName} />
		<button on:click={() => createComponent(componentName)}>New Component</button>
		{#if project?.components}
			<ul>
				{#each project.components as componentId}
					{#await ComponentsService.getComponent(componentId) then component}
						<li>
							<h4>{component.name}</h4>
						</li>
					{/await}
				{/each}
			</ul>
		{/if}
	</div>
</div>
