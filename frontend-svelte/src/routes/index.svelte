<script lang="ts">
	import Projects from './projects/index.svelte';
	import * as signalR from '@microsoft/signalr';
	import { onMount } from 'svelte';
	let usersHubConnection: signalR.HubConnection;
	let backend_base_url = import.meta.env.VITE_BACKEND_BASE_URL;
	let frontend_base_url = import.meta.env.VITE_FRONTEND_BASE_URL;
	onMount(async () => {
		usersHubConnection = new signalR.HubConnectionBuilder()
			.withUrl(backend_base_url + '/api/hubs/users', {
				accessTokenFactory: () => localStorage.getItem('token')
			})
			.configureLogging(signalR.LogLevel.Information)
			.build();
		try {
			await usersHubConnection.start();
		} catch (err) {
			console.log(err);
		}
		usersHubConnection.stream('GetAllOnlineUsers').subscribe({
			next: (p) => {
				console.log(p);
			},
			complete: () => {},
			error: (err) => {
				console.log(err);
			}
		});
	});
</script>
<Projects />
