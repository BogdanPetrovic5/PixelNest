declare module 'maptiler/sdk' {
    export class Map {
        constructor(options: {
            container: string;
            style: string;
            center: [number, number];
            zoom: number;
        });
    }

    export class Marker {
        setLngLat(coordinates: [number, number]): this;
        addTo(map: Map): this;
    }
}