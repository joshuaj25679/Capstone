public class HotelObject{
    public int Id {get;set;}
    public string HotelName {get;set;}
    public string CheckInDate {get;set;}
    public string CheckOutDate {get;set;}
    public string Price {get;set;}
    public string Description {get;set;}

    public HotelObject(int id, string hotelName, string checkIn, string checkOut, string price, string desc){
        Id = id;
        HotelName = hotelName;
        CheckInDate = checkIn;
        CheckOutDate = checkOut;
        Price = price;
        Description = desc;
    }


}