const kinvey = (() => {
    const BASE_URL = 'https://baas.kinvey.com/';
    const APP_KEY = 'kid_rJ9I2TSTV'; // APP KEY HERE
    const APP_SECRET = 'a43075a936d5470f91659c26ac668df8'; // APP SECRET HERE

    function makeAuth(auth) {
        if (auth == 'basic') {
            return {
                'Authorization': `Basic ${btoa(APP_KEY + ':' + APP_SECRET)}`
            };
        } else {
            return {
                'Authorization': `Kinvey 3247122c-acb8-455b-8caf-82e24d216094.KvHBC26Xez+xKOl6vcAA9L/lDga+pqiEHtaTVZf4jFU=`
            };
        }
    }

    function makeRequest(method, collection, endpoint, auth, take, loaded) {
        let query = `/?query={}&sort={"Id":-1}&limit=${take}&skip=${loaded}`;
        return {
            url: BASE_URL + collection + '/' + APP_KEY + '/' + endpoint + query,
            method,
            headers: makeAuth(auth)
        };
    }

    function post(collection, endpoint, auth, data) {
        let req = makeRequest('POST', collection, endpoint, auth);
        req.data = data;
        return $.ajax(req);
    }

    function get(collection, endpoint, auth, take, loaded) {
        return $.ajax(makeRequest('GET', collection, endpoint, auth, take, loaded));
    }

    function update(collection, endpoint, auth, data) {
        let req = makeRequest('PUT', collection, endpoint, auth);
        req.data = data;
        return $.ajax(req);
    }

    function remove(collection, endpoint, auth) {
        return $.ajax(makeRequest('DELETE', collection, endpoint, auth));
    }

    function testGet() {
        return $.ajax({
            type: "Get",
            url: "/MainNews.Cutt.json",
            dataType: "json",
            success: (data) => {
                return data;
                
            }
        });
    }

    return {
        get,
        post,
        update,
        remove,
        testGet
    };
})();