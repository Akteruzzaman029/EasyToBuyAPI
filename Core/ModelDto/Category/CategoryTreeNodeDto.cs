namespace Core.ModelDto.Category
{
    public static class CategoryTreeBuilder
    {
        public static List<CategoryTreeNode> BuildTree(List<CategoryResponseDto> categories)
        {
            if (categories == null || !categories.Any())
                return new List<CategoryTreeNode>();

            // Create all nodes first
            var nodeDictionary = categories.ToDictionary(
                x => x.Id,
                x => new CategoryTreeNode
                {
                    Id = x.Id,
                    Title = x.Name,
                    ParentId = x.ParentId,
                    HasChild = x.HasChild,
                    ProductCount = x.ProductCount,
                    SequenceNo = x.SequenceNo,
                    Children = new List<CategoryTreeNode>()
                });

            var rootNodes = new List<CategoryTreeNode>();

            // Attach child to parent
            foreach (var category in categories.OrderBy(x => x.SequenceNo))
            {
                var currentNode = nodeDictionary[category.Id];

                if (category.ParentId == 0)
                {
                    rootNodes.Add(currentNode);
                }
                else if (nodeDictionary.TryGetValue(category.ParentId, out var parentNode))
                {
                    parentNode.Children.Add(currentNode);
                }
            }

            // Sort recursively
            SortTree(rootNodes);

            return rootNodes;
        }

        private static void SortTree(List<CategoryTreeNode> nodes)
        {
            if (nodes == null || nodes.Count == 0)
                return;

            nodes.Sort((a, b) => a.SequenceNo.CompareTo(b.SequenceNo));

            foreach (var node in nodes)
            {
                if (node.Children != null && node.Children.Count > 0)
                {
                    SortTree(node.Children);
                }
            }
        }
    }

    public class CategoryTreeNode
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public bool HasChild { get; set; }
        public int ProductCount { get; set; }
        public int SequenceNo { get; set; }
        public List<CategoryTreeNode> Children { get; set; } = new List<CategoryTreeNode>();
    }
}
