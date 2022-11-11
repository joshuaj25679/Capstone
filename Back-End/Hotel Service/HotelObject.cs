public class HotelObject{

    public string HotelName {get;set;}
    public string CheckInDate {get;set;}
    public string CheckOutDate {get;set;}
    public string Price {get;set;}
    public string Description {get;set;}

    public HotelObject(string hotelName, string checkIn, string checkOut, string price, string desc){
        HotelName = hotelName;
        CheckInDate = checkIn;
        CheckOutDate = checkOut;
        Price = price;
        Description = desc;
    }


}