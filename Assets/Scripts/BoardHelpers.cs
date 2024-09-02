using Objects.AdvertiseBlock;
using Objects.BlocksContainer;
using Objects.Cell;
using Objects.LockBlock;

public static class BoardHelpers
{
    public static int GetItemsCountModifier(IAdvertiseBlock advertiseBlock, ICell cell)
    {
        if (cell.AdvertiseBlock == null && advertiseBlock != null) return 1;

        if (cell.AdvertiseBlock != null && advertiseBlock == null) return -1;

        return 0;
    }

    public static int GetItemsCountModifier(ILockBlock lockBlock, ICell cell)
    {
        if (cell.LockBlock == null && lockBlock != null) return 1;

        if (cell.LockBlock != null && lockBlock == null) return -1;

        return 0;
    }

    public static int GetItemsCountModifier(IBlockContainer blockContainer, ICell cell)
    {
        if (cell.BlockContainer == null && blockContainer != null) return 1;

        if (cell.BlockContainer != null && blockContainer == null) return -1;

        return 0;
    }
}