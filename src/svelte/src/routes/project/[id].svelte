<script lang="ts">
	import { page } from '$app/stores';
	import { ComponentsService, OpenAPI, ProjectsService, type Component, type Project } from '$lib/api';
	import { userStore } from '$lib/stores';
	import * as signalR from '@microsoft/signalr';
	import { onMount } from 'svelte';
	let componentsHubConnnection: signalR.HubConnection;
	let backend_base_url = import.meta.env.VITE_BACKEND_BASE_URL;
	let project: Project;
	let components: Map<string, Component>;
	let componentName;
	onMount(async () => {
		OpenAPI.TOKEN= async () => {
			return $userStore.token;
		};
		project = await ProjectsService.getProjectById($page.params.id);
		components = new Map(project.components?.map((item) => [item.id, item]));
		try {
			componentsHubConnnection = new signalR.HubConnectionBuilder()
				.withUrl(backend_base_url + '/api/hubs/components', {
					accessTokenFactory: () => $userStore.token
				})
				.configureLogging(signalR.LogLevel.Information)
				.build();
		} catch (err) {
			console.log(err);
		}
		try {
			await componentsHubConnnection.start();
			componentsHubConnnection.stream('Subscribe').subscribe({
				next: async (notification) => {
					console.log(notification);
					if (notification.operation == 'Added' || notification.operation == 'Updated') {
						let component = await ComponentsService.getComponentById(notification.id);
						components.set(notification.id, component);
					} else {
						components.delete(notification.id);
					}
					components = components;
				},
				complete: () => {},
				error: (err) => {
					console.log(err);
				}
			});
		} catch (err) {
			console.log(err);
		}
	});

	async function createComponent(name: string) {
		let component: Component = {
			name: name
		};
		component = await ComponentsService.createComponent(component);
		if (project.components) {
			project.components = [...project.components, component];
		} else {
			project.components = [component];
		}
		components.set(component.id, component);
		await ProjectsService.updateProject(project);
		components = components;
	}

	async function removeComponent(){
		
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
		{#if components}
			<ul>
				{#each [...components.values()] as component}
					<li>
						<h4>{component.name}</h4>
					</li>
				{/each}
			</ul>
		{/if}
	</div>
</div>
