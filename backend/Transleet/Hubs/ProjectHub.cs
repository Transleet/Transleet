using Microsoft.AspNetCore.SignalR;
using Orleans;
using Transleet.Models;

namespace Transleet.Hubs;

public class ProjectHub : Hub
{
    private readonly IClusterClient _clusterClient;

    public ProjectHub(IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
    }

    public async Task SendProjects()
    {

        await Clients.Caller.SendAsync("ReceiveProject");
    }

    public async Task SendEntry(Entry e) 
    {
        await Clients.All.SendAsync("ReceiveTranslation", e.Id, e);
    }

    public async Task SendTranslation(Translation trans)
    {
        await Clients.All.SendAsync("ReceiveTranslation", trans.Id, trans);
    }

    public async Task SendTerm(Term term)
    {
        await Clients.All.SendAsync("ReceiveTerm", term.Id, term);
    }

    public async Task SendTranslationCollection(TranslationCollection collection)
    {
        await Clients.All.SendAsync("ReceiveTranslationCollection", collection.Id, collection);
    }

    public async Task SendAllTranslationCollections(Project project, List<TranslationCollection> collections)
    {
        await Clients.All.SendAsync("ReceiveAllTranslationCollections", project.Id, collections);
    }

    public async Task SendAllTerms(Project project, List<Term> terms)
    {
        await Clients.All.SendAsync("ReceiveAllTerms", project.Id, terms);
    }
}