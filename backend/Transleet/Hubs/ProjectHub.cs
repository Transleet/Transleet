using Microsoft.AspNetCore.SignalR;
using Transleet.Models;

namespace Transleet.Hubs;

public class ProjectHub:Hub
{
    public async Task UpdateEntry(Entry e)
    {
        await Clients.All.SendAsync("ReceiveUpdateTranslation",e.Id,e);
    }

    public async Task UpdateTranslation(Translation trans)
    {
        await Clients.All.SendAsync("ReceiveUpdateTranslation", trans.Id, trans);
    }

    public async Task UpdateTerm(Term term)
    {
        await Clients.All.SendAsync("ReceiveUpdateTerm", term.Id, term);
    }

    public async Task UpdateTranslationCollection(TranslationCollection collection)
    {
        await Clients.All.SendAsync("ReceiveUpdateTranslationCollection", collection.Id, collection);
    }

    public async Task UpdateTranslationCollections(Project project, List<TranslationCollection> collections)
    {
        await Clients.All.SendAsync("ReceiveUpdateTranslationCollections", project.Id, collections);
    }

    public async Task UpdateTerms(Project project,List<Term> terms)
    {
        await Clients.All.SendAsync("ReceiveUpdateAllTerms", project.Id, terms);
    }
}