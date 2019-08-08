

const mainNewsAPI = (() => {
    const newsApiBaseUrl = `https://newsapi.org/v2/`;
    const newsApiKey = `10040d5f89e648ed80ec85f2aadd4af3`;
    const newsApiContry = "bg";
    const newsApiSort = "popularity";
    const newsApiPublicationsCount = 10;
    const _MS_PER_DAY = 1000 * 60 * 60 * 24;


    function requstConfig(page) {
        let query =
            `top-headlines?country=bg&apiKey=10040d5f89e648ed80ec85f2aadd4af3&pageSize=${newsApiPublicationsCount}`;
            
        if (page >= 1) {
            query = query + `&page=${page+1}`;
        }

        return {
            url: newsApiBaseUrl + query,
            method: "GET",
            crossDomain:true
        }

    }

    function getTopHeadlinesInBg(page) {
        return $.ajax(requstConfig(page));
    }

    function dateDiffInDays(a) {
        // Discard the time and time-zone information.
        let date = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
        let today = new Date();
        today = Date.UTC(today.getFullYear(), today.getMonth(), today.getDate());
        let diff =  Math.floor((today - date) / _MS_PER_DAY);

        switch (diff) {
        case 0:
            return "днес";
        case 1:
            return "вчера";
        default:
            return `преди ${diff} дни`;
        }

    }



    return {
        requstConfig,
        getTopHeadlinesInBg,
        dateDiffInDays
    }

})();

