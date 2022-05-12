namespace Router.TpLink;

public record RouterHttpMessage(string Path,
                                IEnumerable<KeyValuePair<string, string>>? Query = null,
                                HttpMethod? Method = null);