export type ApartamentPostDto = {
    address:string;
    area:number;
    latitude:number;
    longtitude:number;
    bedrooms:number;
}

export type ApartamentGetDto = {
    apartamentId:number;
    address:string;
    area:number;
    bedrooms:number;
    latitude:number;
    longtitude:number;
    hostName:string;
}

