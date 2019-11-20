using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using CS4227_Database_API.Models.Containers;

namespace CS4227_Database_API.Models.DBContexts.Interaction_Classes
{
    interface IInteraction
    {
        #region GetDescription
        /* <Summary>
         *   Methods below are used for retrieving data from the Database
         * </Summary>
         * <Description>
         *   **Reference to list passed in constructor.**
         *   GetAll - Requires and returns nothing.
         *   GetMultipleEntries - Requires an IDBContainer List of Entries Requested, Returns nothing.
         *   GetSingleEntries - Requires an IDBContainer of Entry Requested, Returns IDBContainer if Entry Exists.
         * </Description>
         */
        #endregion
        void GetAll();
        void GetMultipleEntries(List<IDBContainer> Values);
        void GetPlayerEntry(IDBContainer Values);

        #region AddDescription
        /* <Summary>
         *   Methods below are used for Inserting Entries to the Database
         * </Summary>
         * <Description>
         *   **Reference to list passed in constructor.**
         *   AddEntry - Requires an IDBContainer for Entry to Add, Returns Success String.
         *   (Overload)AddEntry - Requires a IDBContainer List of Entries to Add, Returns Success String.
         * </Description>
         */
        #endregion
        void AddEntry(IDBContainer Value);
        void AddEntry(List<IDBContainer> Values);

        #region UpdateDescription
        /* <Summary>
         *   Methods below are used for Altering Entries in the Database
         * </Summary>
         * <Description>
         *   **Reference to list passed in constructor.**
         *   UpdateEntry - Requires an IDBContainer for Entry to Alter, Returns Success String.
         *   (Overload)UpdateEntry - Requires a IDBContainer List of Entries to Alter, Returns Success String.
         * </Description>
         */
        #endregion
        void UpdateEntry(IDBContainer Value);
        void UpdateEntry(List<IDBContainer> Values);

        #region DeleteDescription
        /* <Summary>
         *   Methods below are used for Inserting Entries to the Database
         * </Summary>
         * <Description>
         *   **Reference to list passed in constructor.**
         *   DeleteEntry - Requires an IDBContainer for Entry to Delete, Returns Success String.
         *   (Overload)DeleteEntry - Requires a IDBContainer List of Entries to Delete, Returns Success String.
         * </Description>
         */
        #endregion
        void DeleteEntry(IDBContainer Value);
        void DeleteEntry(List<IDBContainer> Values);
    }
}
