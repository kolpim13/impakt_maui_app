using impakt_maui_app.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Models
{
    public enum AccountType : ushort
    {
        Root = 0,
        Admin = 1,
        Instructor = 2,
        Member = 3,
        External = 4,
    }
    public class Model_Member
    {
        required public string CardId { get; set; }
        required public string Name { get; set; }
        required public string Surname { get; set; }
        required public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        required public DateOnly RegistrationDate { get; set; }
        required public AccountType AccountType { get; set; }
        public string? Privileges { get; set; }
        public bool? LastCheckInSsuccess { get; set; }
        public DateTime? LastCheckInDateTime { get; set; }
        public string? Token { get; set; }
        required public bool Activated { get; set; }
        public static Model_Member From_Resp_Inst(Resp_Members_Inst inst) =>
            new Model_Member
            {
                CardId = inst.card_id,
                Name = inst.name,
                Surname = inst.surname,
                Email = inst.email,
                PhoneNumber = inst.phone_number,
                DateOfBirth = inst.date_of_birth,
                RegistrationDate = inst.registration_date,
                AccountType = (AccountType)inst.account_type,
                Privileges = inst.privileges,
                LastCheckInSsuccess = inst.last_checkin_success,
                LastCheckInDateTime = inst.last_checkin_datetime,
                Token = inst.token,
                Activated = inst.activated,
            };
        public static Model_Member GetDefaultInst() =>
            new Model_Member
            {
                CardId = "",
                Name = "",
                Surname = "",
                Email = "",
                RegistrationDate = DateOnly.FromDateTime(DateTime.Today),
                AccountType = AccountType.Member,
                Activated = false,
            };
    }

    public class Model_ExternalProvider
    {
        required public int Id { get; set; }
        required public string Name { get; set; }
        public string? Description { get; set; }
        required public bool IsPartialPayment { get; set; }
        public decimal? PartialPayment { get; set; }
        required public bool IsDeleted { get; set; }

        public static Model_ExternalProvider From_Resp_Inst(Resp_Instance_ExternalProviders inst) =>
           new Model_ExternalProvider
           {
               Id = inst.id,
               Name = inst.name,
               Description = inst.description,
               IsPartialPayment = inst.is_partial_payment,
               PartialPayment = inst.partial_payment,
               IsDeleted = inst.is_deleted,
           };
    }

    public class Model_PassType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? ValidityDays { get; set; }
        public int? MaximumEntries { get; set; }
        public bool RequiresExternalAuth { get; set; }
        public string? ExternalProviderName { get; set; }
        public int? ExternalProviderId { get; set; }
        public bool IsExtEventPass { get; set; }
        public string? ExtEventCode { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }

        public static Model_PassType From_Resp_Inst(Resp_PassTypes_Inst inst) =>
            new Model_PassType
            {
                Id = inst.id,
                Name = inst.name,
                Description = inst.description,
                Price = inst.price,
                ValidityDays = inst.validity_days,
                MaximumEntries = inst.maximum_entries,
                RequiresExternalAuth = inst.requires_external_auth,
                ExternalProviderName = inst.external_provider_name,
                ExternalProviderId = inst.external_provider_id,
                IsExtEventPass = inst.is_ext_event_pass,
                ExtEventCode = inst.ext_event_code,
                IsDeleted = inst.is_deleted,
                DeleteDate = inst.delete_date,
            };
    }

    public class Model_MemberPass
    {
        required public int Id { set; get; }
        required public string MemberCardId { get; set; }
        required public int PassTypeId { get; set; }
        required public string PassTypeName { get; set; }
        required public DateOnly PurchaseDate { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public int? EntriesLeft { get; set; }
        required public bool RequiresExternalAuth { get; set; }
        public int? ExternalProviderId { get; set; }
        public string? ExternalProviderName { get; set; }
        required public bool IsExtEventPass { get; set; }
        public string? ExtEventCode { get; set; }
        public string? Status { get; set; }
        required public bool IsClosed { get; set; }
        public static Model_MemberPass From_Resp_Inst(Resp_MemberPass_Inst inst) =>
            new Model_MemberPass
            {
                Id = inst.id,
                MemberCardId = inst.member_card_id,
                PassTypeId = inst.pass_type_id,
                PassTypeName = inst.pass_type_name,
                PurchaseDate = inst.purchase_date,
                ExpirationDate = inst.expiration_date,
                EntriesLeft = inst.entries_left,
                RequiresExternalAuth = inst.requires_external_auth,
                ExternalProviderId = inst.external_provider_id,
                ExternalProviderName = inst.external_provider_name,
                IsExtEventPass = inst.is_ext_event_pass,
                ExtEventCode = inst.ext_event_code,
                Status = inst.status,
                IsClosed = inst.is_closed,
            };
    }

    public class Model_Checkin
    {
        required public int Id { get; set; }
        public string? ValidatedByCardId { get; set; }
        public string? ValidatedByName { get; set; }
        public string? ValidatedBySurname{ get; set; }
        public string? Hall { get; set; }
        public int? MemberPassId { get; set; }
        public int? PassId { get; set; }
        public string? PassName{ get; set; }
        public bool? IsExtEventPass{ get; set; }
        public string? ExtEventCode { get; set; }
        public int? ExternalProviderId { get; set; }
        public string? ExternalProviderName { get; set; }
        required public string MemberCardID { get; set; }
        required public string MemberName { get; set; }
        required public string MemberSurname { get; set; }
        required public DateTime DateTime { get; set; }
        required public bool IsSuccessful { get; set; }
        public string? RejectedReason { get; set; }

        public static Model_Checkin From_resp_Inst(Resp_ChecIn_Inst inst) =>
            new Model_Checkin
            {
                Id = inst.id,
                ValidatedByCardId = inst.validated_by_card_id,
                ValidatedByName = inst.validated_by_name,
                ValidatedBySurname = inst.validated_by_surnamename,
                Hall = inst.hall,
                MemberPassId = inst.member_pass_id,
                PassId = inst.pass_id,
                PassName = inst.pass_name,
                IsExtEventPass = inst.is_ext_event_pass,
                ExtEventCode = inst.ext_event_code,
                ExternalProviderId = inst.external_provider_id,
                ExternalProviderName = inst.external_provider_name,
                MemberCardID = inst.member_card_id,
                MemberName = inst.member_name,
                MemberSurname = inst.member_surname,
                DateTime = inst.date_time,
                IsSuccessful = inst.is_successful,
                RejectedReason = inst.rejected_reason,
            };
    }
}
