using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessSpecificLogic.FS.Customer
{
    public class FSItem : FSEntity
    {
        [NotMapped]
        public override int id { get { return ItemKey; } }

        [NotMapped]
        public string Value { get { return ItemNumber; } }

        public int ItemKey { get; set; }
        public string ItemNumber { get; set; }
        public string ItemDescription { get; set; }
        public string ItemReference1 { get; set; }
        public string ITEM_REF1_DESC { get; set; }
        public int CustomerKey { get; set; }

        public override string sqlGetAll
        {
            get
            {
                return @"SELECT DISTINCT _NoLock_FS_Item.ItemNumber
	                        ,_NoLock_FS_Item.ItemDescription
	                        ,_NoLock_FS_Item.ItemReference1
	                        ,_CAP_Class_Ref.ITEM_REF1_DESC
	                        ,_NoLock_FS_Item.ItemKey
                        FROM _NoLock_FS_Customer
                        INNER JOIN _NoLock_FS_COHeader ON _NoLock_FS_Customer.CustomerKey = _NoLock_FS_COHeader.CustomerKey
                        INNER JOIN _NoLock_FS_COLine ON _NoLock_FS_COHeader.COHeaderKey = _NoLock_FS_COLine.COHeaderKey
                        INNER JOIN _NoLock_FS_Item ON _NoLock_FS_COLine.ItemKey = _NoLock_FS_Item.ItemKey
                        LEFT OUTER JOIN _CAP_Class_Ref ON _NoLock_FS_Item.ItemReference1 = _CAP_Class_Ref.ITEM_REF1
                        ORDER BY _NoLock_FS_Item.ItemNumber";
            }
        }

        public override string sqlGetById
        {
            get
            {
                return @"SELECT DISTINCT _NoLock_FS_Item.ItemNumber
	                        ,_NoLock_FS_Item.ItemDescription
	                        ,_NoLock_FS_Item.ItemReference1
	                        ,_CAP_Class_Ref.ITEM_REF1_DESC
	                        ,_NoLock_FS_Item.ItemKey
                        FROM _NoLock_FS_Customer
                        INNER JOIN _NoLock_FS_COHeader ON _NoLock_FS_Customer.CustomerKey = _NoLock_FS_COHeader.CustomerKey
                        INNER JOIN _NoLock_FS_COLine ON _NoLock_FS_COHeader.COHeaderKey = _NoLock_FS_COLine.COHeaderKey
                        INNER JOIN _NoLock_FS_Item ON _NoLock_FS_COLine.ItemKey = _NoLock_FS_Item.ItemKey
                        LEFT OUTER JOIN _CAP_Class_Ref ON _NoLock_FS_Item.ItemReference1 = _CAP_Class_Ref.ITEM_REF1
                        WHERE _NoLock_FS_Item.ItemKey = @id";
            }
        }
    }
}
