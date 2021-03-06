﻿using System;

namespace Yunchee.Volkswagen.Entity.Interface
{
    public interface ICustomTree
    {
        /// <summary>
        /// ID
        /// </summary>
        Int32? ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 菜单在树形结构中的层级（从0开始）
        /// </summary>
        int TreeLevel { get; set; }

        /// <summary>
        /// 是否可用（默认true）,在模拟树的下拉列表中使用
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// 是否叶子节点（默认true）
        /// </summary>
        bool IsTreeLeaf { get; set; }
    }
}
