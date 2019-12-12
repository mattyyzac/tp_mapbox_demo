let lang = '';

function rendermap(loc) {
    if (!loc || loc === null) { return; }

    mapboxgl.accessToken = document.getElementById('mapboxtoken').value;

    const map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [ loc.coords.longitude, loc.coords.latitude ],
        zoom: 7 // starting zoom
    });

    const nav = new mapboxgl.NavigationControl();
    map.addControl(nav, 'bottom-right');

    map.on('load', () => {
        mapcallback(map);
    });
}

function mapcallback(map) {
    map.addSource('sceneries', {
        type: 'geojson',
        data: `${apiRoot}/sceneries`,
        cluster: true, // Enable clustering
        clusterRadius: 50, // Radius of each cluster when clustering points
        clusterMaxZoom: 6 // Max zoom to cluster points on
    });

    map.addLayer({
        id: 'sceneries',
        type: 'circle',
        source: 'sceneries',
        filter: ['!has', 'point_count'],
        paint: {
            'circle-color': '#a890f0',
            'circle-radius': 5,
            'circle-stroke-width': 1,
            'circle-stroke-color': '#fff'
        }
    });

    map.on('mouseenter', 'sceneries', (data) => {
        map.getCanvas().style.cursor = 'pointer';

        const name = data.features[0].properties.name;

        $('#card').modal('toggle');

        const api = `/api/g/r/${name}`;
        fetch(api)
            .then(resp => {
                if (resp.status === 200) {
                    return resp.json();
                }
                return {};
            })
            .then(data => {
                setSceneryProperties(data);
            })
            .catch(error => {
                console.log(error);
                document.getElementById('scenery-image').src = 'https://via.placeholder.com/400x200?text=Error+while+loading+data';
            });
    });

    map.on('mouseleave', 'sceneries', () => {
        map.getCanvas().style.cursor = '';
        closeInfoCard();
    });

    var placesAutocomplete = places({
        container: document.querySelector('#where-to-go'),
        type: 'city'
    });

    placesAutocomplete.on('change', e => {
        map.flyTo({
            center: [e.suggestion.latlng.lng, e.suggestion.latlng.lat],
            zoom: 9
        });
    });
}

function setSceneryProperties(data) {
    if (!data || data === null) { return; }

    if (data.photo && data.photo !== null) {
        document.getElementById('scenery-image').src = `data:image/png;base64,${data.photo}`;
    }
    else {
        document.getElementById('scenery-image').src = 'https://via.placeholder.com/500x300?text=No+Image+Found';
    }

    document.getElementById('name').innerText = data.name || '';
    document.getElementById('address').innerText = data.formattedAddress || '';
    document.getElementById('website').innerHTML = data.website || '';
}

function closeInfoCard() {
    document.getElementById('scenery-image').src = 'https://via.placeholder.com/500x300?text=loading';
    document.getElementById('name').innerText = '';
    document.getElementById('address').innerText = '';
    document.getElementById('website').innerHTML = '';

    $('#card').modal('toggle');
}

function getBaseUri(isApi = false) {
    const loc = window.location;
    const protocol = loc.protocol;
    const host = loc.host;
    if (isApi) {
        return `${protocol}//${host}/api`;
    }
    return `${protocol}//${host}`;
}

const apiRoot = getBaseUri(true);

(() => {
    if (navigator.geolocation) {
        // we get geolocation from browsers, to make map centered upon current location.
        navigator.geolocation.getCurrentPosition(rendermap);
    }
    // what if, geolocation did NOT support by browsers, or premission denied by users !?
})();
