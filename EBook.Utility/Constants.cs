namespace EBook.Utility;
public static class Constants
{
    //End users
    public const string RoleUserIndividual = "Individual";
    public const string RoleUserCompany = "Company";

    //Roles for managment the content of the website
    public const string RoleAdmin = "Admin";
    public const string RoleEmployee = "Employee";

    //Status for shipped orders
    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusInProcess = "Processing";
    public const string StatusShipped = "Shipped";
    public const string StatusCancelled = "Cancelled";
    public const string StatusRefunded = "Refunded";

    //Status for payment
    public const string PaymentStatusPending = "Pending";
    public const string PaymentStatusApproved = "Approved";
    public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
    public const string PaymentStatusRejected = "Rejected";
}
