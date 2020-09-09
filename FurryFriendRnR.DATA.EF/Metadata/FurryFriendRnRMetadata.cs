using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FurryFriendRnR.DATA.EF
{

    #region Location

    public class LocationMetadata
    {
        [Required(ErrorMessage = "* Location is required.")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "* Location Name is required")]
        [StringLength(50, ErrorMessage = "Location Name must be 50 characters or less")]
        [Display(Name = "Location Name:")]
        public string LocationName { get; set; }
        
        [StringLength(100, ErrorMessage = "* Street Address must be 100 characters or less.")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "Address:")]
        public string Address { get; set; }

        [Required(ErrorMessage = "* City is required")]
        [StringLength(100, ErrorMessage = "City must be 100 characters or less")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "City:")]
        public string City { get; set; }

        [Required(ErrorMessage = "* State is required")]
        [StringLength(2, ErrorMessage = "State must be 2 characters", MinimumLength = 2)]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "State:")]
        public string State { get; set; }

        [Required(ErrorMessage = "* Zip Code is required")]
        [StringLength(5, ErrorMessage = "Zip Code must be 5 characters", MinimumLength = 5)]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "Zip Code:")]
        public string ZipCode { get; set; }

        [Display(Name = "Daily Limit:")]
        [Required(ErrorMessage = "* Limit is required")]
        public byte ReservationLimit { get; set; }
    }

    [MetadataType(typeof(LocationMetadata))]
    public partial class Location
    {

    }

    #endregion


    #region OwnerAsset

    public class OwnerAssetMetadata
    {

        public int OwnerAssetId { get; set; }

        [Required(ErrorMessage = "* Furry Friends Name is required")]
        [StringLength(50, ErrorMessage = "Furry Friends Name must be 50 characters or less")]
        [Display(Name = "Furry Friends Name:")]
        public string PetName { get; set; }
        
        public string OwnerId { get; set; }

        [Display(Name = "Photo:")]
        public string AssetPhoto { get; set; }

        [UIHint("MultilineText")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "Special Notes:")]
        public string SpecialNotes { get; set; }

        [Display(Name = "Current Customer:")]
        public bool IsActive { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "[N/A]")]
        [Display(Name = "Date Added:")]
        public System.DateTime DateAdded { get; set; }

        [Required(ErrorMessage = "* Pet Breed is required")]
        [StringLength(50, ErrorMessage = "Pet Breed must be 50 characters or less")]
        [Display(Name = "Pet Breed:")]
        public string PetBreed { get; set; }

        [Display(Name = "Records:")]
        public string RecordFile { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "[N/A]")]
        [Display(Name = "Pets Birthdate:")]
        [Required(ErrorMessage = "* Birthdate is required")]
        public System.DateTime PetBirthdate { get; set; }

        [Display(Name = "Weight:")]
        [Required(ErrorMessage = "* Pet Weight is required")]
        public int Weight { get; set; }

        [Required(ErrorMessage = "* Pet Coat Color is required")]
        [StringLength(50, ErrorMessage = "Pet Coat Color must be 50 characters or less")]
        [Display(Name = "Pet Coat Color:")]
        public string Color { get; set; }

        [Display(Name = "Spayed or Neutered:")]
        [Required(ErrorMessage = "* Fixed is required")]
        public bool IsFixed { get; set; }

        [Display(Name = "Male or Female:")]
        [Required(ErrorMessage = "* Gender is required")]
        public bool PetGender { get; set; }
    }

    [MetadataType(typeof(OwnerAssetMetadata))]
    public partial class OwnerAsset
    {

    }

    #endregion


    #region Reservation

    public class ReservationMetadata
    {
        [Required(ErrorMessage = "* Reservation ID is required.")]
        public int ReservationId { get; set; }

        [Required(ErrorMessage = "* Pet ID is required.")]
        public int OwnerAssetId { get; set; }

        [Required(ErrorMessage = "* Location is required.")]
        public int LocationId { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "[N/A]")]
        [Display(Name = "Reservation Date:")]
        public System.DateTime ReservationDate { get; set; }

    }

    [MetadataType(typeof(ReservationMetadata))]
    public partial class Reservation
    {

    }

    #endregion


    #region UserDetail

    public class UserDetailMetadata
    {
        [Required(ErrorMessage = "* Owner ID is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "* First Name is required")]
        [StringLength(50, ErrorMessage = "First Name must be 50 characters or less")]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "* Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name must be 50 characters or less")]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "* Street Address is required")]
        [StringLength(100, ErrorMessage = "Street Address must be 100 characters or less")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "Address:")]
        public string Address { get; set; }

        [Required(ErrorMessage = "* City is required")]
        [StringLength(100, ErrorMessage = "City must be 100 characters or less")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "City:")]
        public string City { get; set; }

        [Required(ErrorMessage = "* State is required")]
        [StringLength(2, ErrorMessage = "State must be 2 characters", MinimumLength = 2)]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "State:")]
        public string State { get; set; }

        [Required(ErrorMessage = "* Zip Code is required")]
        [StringLength(5, ErrorMessage = "Zip Code must be 5 characters", MinimumLength = 5)]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Display(Name = "Zip Code:")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "* Phone Number is required")]
        [Display(Name = "Phone Number:")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail
    {
        [Display(Name = "Owner:")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

    }

    #endregion

}
