// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.5.0.21210
//    <NameSpace>NStub.CSharp.ObjectGeneration.Builders</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>True</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>True</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><VirtualProp>False</VirtualProp><PascalCase>False</PascalCase><BaseClassName>EmptyMultiBuildParametersBase</BaseClassName><IncludeSerializeMethod>True</IncludeSerializeMethod><UseBaseClass>True</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net35</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>False</GenerateXMLAttributes><OrderXMLAttrib>False</OrderXMLAttrib><EnableEncoding>False</EnableEncoding><AutomaticProperties>True</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>True</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>ASCII</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><InitializeFields>Collections</InitializeFields><GenerateAllTypes>True</GenerateAllTypes>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace NStub.CSharp.ObjectGeneration.Builders {
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    
    
    public partial class MultiBuildParameters : EmptyMultiBuildParametersBase<MultiBuildParameters> {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<MultiBuildParametersOfRenamingBuilder> itemsField;
        
        public List<MultiBuildParametersOfRenamingBuilder> Items {
            get {
                if ((this.itemsField == null)) {
                    this.itemsField = new List<MultiBuildParametersOfRenamingBuilder>();
                }
                return this.itemsField;
            }
        }
    }
    
    public partial class MultiBuildParametersOfRenamingBuilder : EmptyMultiBuildParametersBase<MultiBuildParametersOfRenamingBuilder> {
        
        public string MethodSuffix { get; set; }
        public bool UseDings { get; set; }
        public int Moep { get; set; }
    }
}
